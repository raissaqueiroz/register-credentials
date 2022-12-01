using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;
using StoneCo.FinancialPositionHub.Application.Repositories;
using StoneCo.FinancialPositionHub.Domain.Model;
using RegisterCredentials.Infra.Repository;
using RegisterCredentials.Infra.Service.DistributedLock;
using RegisterCredentials.Infra.Service.DistributedLock.Interfaces;

namespace RegisterCredentials.Infra.Extensions
{
    public static class MongoDbConnectionExtensions
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
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var mongoUrl = MongoUrl.Create(connectionString);
            var client = new MongoClient(mongoUrl);

            return client.GetDatabase(mongoUrl.DatabaseName);
        }

        private static void ConfigureMappers()
        {
            lock (_mapSyncRoot)
            {
                if (BsonClassMap.IsClassMapRegistered(typeof(Receivable)) is false)
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
                }
            }
        }
        
        public static async IAsyncEnumerable<TDocument> ToCursorAsyncEnumerable<TDocument>(
            this IAsyncCursor<TDocument> source,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Ensure.IsNotNull(source, nameof (source));
            
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
            services.AddScoped(x => db.GetCollection<Receivable>("receivables"));
            services.AddScoped(x => db.GetCollection<ReceivableFile>("receivableFiles"));
            services.AddScoped(x => db.GetCollection<Reconciliation>("reconciliations"));
            services.AddScoped(x => db.GetCollection<ReceivableLog>("receivableLogs"));
            services.AddScoped(x => db.GetCollection<ArrangedFile>("arangedFiles"));
            services.AddScoped(x => db.GetCollection<ArrangedFilesJobExecution>("arrangedFilesJobExecutions"));
            services.AddScoped(x => db.GetCollection<MongoDbExclusiveLock>("exclusiveLocks").WithReadPreference(ReadPreference.Primary));
        }

        private static void ConfigureRepositories(IServiceCollection services, IMongoDatabase db)
        {
            services.AddScoped<IReceivableRepository, ReceivableRepository>();
            services.AddScoped<IReceivableFileRepository, ReceivableFileRepository>();
            services.AddScoped<IReceivableLogRepository, ReceivableLogRepository>();
            services.AddScoped<IReconciliationRepository, ReconciliationRepository>();
            services.AddScoped<IArrangedFileRepository, ArrangedFileRepository>();
            services.AddScoped<IArrangedFilesJobExecutionRepository, ArrangedFilesJobExecutionRepository>();
            services.AddScoped<IDistributedLockService, MongoDbDistributedLockService>();
            services.AddScoped<IMongoDbStatsRepository, MongoDbStatsRepository>(sp => new MongoDbStatsRepository(db.WithReadPreference(ReadPreference.Primary)));
        }
    }
}
