using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestionDesRessourcesMaterielles.Data.BackgroundServices
{
    public class LivredUpdaterBackgroundService : IHostedService
    {
        private readonly ILogger<LivredUpdaterBackgroundService> _logger;
        private readonly ApplicationDbContext _authContext;
        private Timer _timer;

        public LivredUpdaterBackgroundService(ILogger<LivredUpdaterBackgroundService> logger, ApplicationDbContext authContext)
        {
            _logger = logger;
            _authContext = authContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Livred updater is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(2));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Livred updater running at: {time}", DateTimeOffset.Now);

            try
            {
                var currentTime = DateTime.UtcNow;

                var ordinateursToUpdate = await _authContext.Ordinateurs
                    .Where(o => o.Livred != true && o.DateLivraison <= currentTime)
                    .ToListAsync();

                foreach (var ordinateur in ordinateursToUpdate)
                {
                    ordinateur.Livred = true;
                }

                var imprimantesToUpdate = await _authContext.Imprimantes
                    .Where(i => i.Livred != true && i.DateLivraison <= currentTime)
                    .ToListAsync();

                foreach (var imprimante in imprimantesToUpdate)
                {
                    imprimante.Livred = true;
                }

                await _authContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating livred resources");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Livred updater is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
