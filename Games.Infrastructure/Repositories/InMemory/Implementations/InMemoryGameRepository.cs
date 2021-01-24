using Games.Core.Entties.Game;
using Games.Core.Interfaces.Repositories.Games;
using Games.Infrastructure.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Games.Infrastructure.Repositories.InMemory.Implementations
{
    internal class InMemoryGameRepository : InMemoryBaseRepository<Game>, IGameRepository
    {
        public InMemoryGameRepository() : base()
        {
            var game1 = new Game(Guid.NewGuid().ToString("N"), "Cyber Punk 2077", "Gry roleplaying mrocznej przyszłości, tworzonej przez autorów serii gier o Wiedźminie, studio CD PROJEKT RED.", "Gra akcji", "https://www.notebookcheck.net/fileadmin/Notebooks/News/_nc3/cyberpunk207737.jpg", DateTime.UtcNow);
            var game2 = new Game(Guid.NewGuid().ToString("N"), "Deluxe SKI Jump2", "Deluxe Ski Jump to druga część popularnego symulatora skoków narciarskich", "Sportowa", "https://www.instalki.pl/images/newsy/11-2020/dsj-2-android-01.jpg", DateTime.UtcNow);
            var game3 = new Game(Guid.NewGuid().ToString("N"), "Fifa 21", "Opis C", "FIFA 21 to gra wideo symulująca piłkę nożną wydana przez Electronic Arts w ramach serii FIFA.", "https://r.dcs.redcdn.pl/http/o2/redefine/cp/s5/s5yexaz94marchp8o2r4vdf4ahrvv99s.jpg", DateTime.UtcNow);
            var game4 = new Game(Guid.NewGuid().ToString("N"), "Wiedźmin 3 dziki gon", "Trzecia i ostatnia odsłona serii gier komputerowych Wiedźmin, opowiadającej historię Geralta z Rivii, studia CD Projekt RED. Historia gry toczy się po wydarzeniach z drugiej części.", "Akcja", "https://static.wikia.nocookie.net/wiedzmin/images/3/34/O_W3.jpg/revision/latest/scale-to-width-down/960?cb=20160104182402", DateTime.UtcNow);

            Init(new List<Game> { game1, game2, game3, game4 });
        }

        [TrackDependency]
        public virtual async Task<int> CountByParameters(QueryParameters parameters, CancellationToken cancellationToken)
        {
            var result = Items
                 .Values
                 .AsQueryable()
                 .Where(() => !string.IsNullOrEmpty(parameters.Name), x => x.Name.Contains(parameters.Name, StringComparison.InvariantCultureIgnoreCase))
                 .Where(() => !string.IsNullOrEmpty(parameters.Category), x => x.Category.Contains(parameters.Category, StringComparison.InvariantCultureIgnoreCase))
                 .Where(() => !string.IsNullOrEmpty(parameters.Description), x => x.Description.Contains(parameters.Description, StringComparison.InvariantCultureIgnoreCase))
                 .Count();
            
            return await Task.FromResult(result);
        }

        [TrackDependency]
        public virtual async Task<IList<Game>> QueryByParameters(QueryParameters parameters, CancellationToken cancellationToken)
        {
            var orderByProperty = parameters.OrderBy != null
                ? OrderByPropertues.FirstOrDefault(x => x.Equals(parameters.OrderBy, StringComparison.InvariantCultureIgnoreCase))
                : default;
            var result = Items
                .Values
                .AsQueryable()
                .Where(() => !string.IsNullOrEmpty(parameters.Name), x => x.Name.Contains(parameters.Name, StringComparison.InvariantCultureIgnoreCase))
                .Where(() => !string.IsNullOrEmpty(parameters.Category), x => x.Category.Contains(parameters.Category, StringComparison.InvariantCultureIgnoreCase))
                .Where(() => !string.IsNullOrEmpty(parameters.Description), x => x.Description.Contains(parameters.Description, StringComparison.InvariantCultureIgnoreCase))
                .Where(() => parameters.ReleaseDate != null, x => x.ReleaseDate <= parameters.ReleaseDate)
                .OrderBy(orderByProperty, parameters.IsDescending)
                .GetPage(parameters.PageSize, parameters.PageIndex);

            return await Task.FromResult(result);
        }

        private readonly string[] OrderByPropertues = new string[] { "Name", "Description", "Category", "ImageUrl", "ReleaseDate", "AvarageRating" };
    }
}
