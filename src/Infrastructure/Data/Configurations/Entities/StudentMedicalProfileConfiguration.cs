using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using SchoolTripApi.Domain.StudentMedicalProfileAggregate;
using SchoolTripApi.Infrastructure.Data.Abstractions;

namespace SchoolTripApi.Infrastructure.Data.Configurations.Entities;

public class StudentMedicalProfileConfiguration : IMongoEntityConfiguration
{
    public void Configure()
    {
        BsonClassMap.RegisterClassMap<StudentMedicalProfile>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(smp => smp.Id)
                .SetIdGenerator(GuidGenerator.Instance)
                .SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
        });
    }
}