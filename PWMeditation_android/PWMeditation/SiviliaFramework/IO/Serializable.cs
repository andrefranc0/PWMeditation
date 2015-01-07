
namespace SiviliaFramework.IO
{
    public interface ISerializableType
    {
        void Serialize(DataStream ds);
    }
    public interface IDeserializableType
    {
        void Deserialize(DataStream ds);
    }
}
