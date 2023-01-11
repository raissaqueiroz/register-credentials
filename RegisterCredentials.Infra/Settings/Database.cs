using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;
using System.Runtime.CompilerServices;

namespace StoneCo.FinancialPositionHub.Infra.Settings
{
    public static class Database
    {
        private static object _mapSyncRoot = new object();

        public static IServiceCollection ConfigureMongoDbRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var db = GetMongoDatabase(configuration);

            var pack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("", pack, t => true);

            ConfigureMappers();
            ConfigureCollections(services, db);
            ConfigureRepositories(services, db);

            return services;
        }

        private static IMongoDatabase GetMongoDatabase(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("Database:ConnectionString").Value;
            var mongoUrl = MongoUrl.Create(connectionString);
            var client = new MongoClient(mongoUrl);

            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        private static void ConfigureMappers()
        {
            lock (_mapSyncRoot)
            {
                /*if (BsonClassMap.IsClassMapRegistered(typeof(Receivable)) is false)
                {
                    BsonClassMap.RegisterClassMap<Receivable>(map =>
                    {
                        map.AutoMap();
                        map.SetIgnoreExtraElements(true);
                        map.MapField(z => z.ProcessingDetails).SetDefaultValue(new ReceivableProcessingDetails());
                    });
                }

                if (BsonClassMap.IsClassMapRegistered(typeof(ContractObligation)) is false)
                {
                    BsonClassMap.RegisterClassMap<ContractObligation>(map =>
                    {
                        map.AutoMap();
                        map.SetIgnoreExtraElements(true);
                    });
                }*/
            }
        }

        public static async IAsyncEnumerable<TDocument> ToCursorAsyncEnumerable<TDocument>(
            this IAsyncCursor<TDocument> source,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(source, nameof(source));

            using (source)
            {
            label_9:
                if (await source.MoveNextAsync(cancellationToken).ConfigureAwait(false))
                {
                    using IEnumerator<TDocument> enumerator = source.Current.GetEnumerator();

                    while (enumerator.MoveNext())
                    {
                        yield return enumerator.Current;

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    goto label_9;
                }
            }
        }

        private static void ConfigureCollections(IServiceCollection services, IMongoDatabase db)
        {
            //Example: services.AddScoped(x => db.GetCollection<Receivable>("receivables"));
         
        }

        private static void ConfigureRepositories(IServiceCollection services, IMongoDatabase db)
        {
            //Exemple: services.AddScoped<IReceivableRepository, ReceivableRepository>();

            //services.AddScoped<IMongoDbStatsRepository, MongoDbStatsRepository>(sp => new MongoDbStatsRepository(db.WithReadPreference(ReadPreference.Primary)));
        }
    }
}
