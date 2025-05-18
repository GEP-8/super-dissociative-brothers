// using System;
// using Unity.Netcode;
//
// namespace Networks {
//     public interface INetworkEvent : INetworkSerializable {
//         public string Name => GetType().Name;
//     }
//
//     public class NetworkEvent<T> : INetworkEvent, IEquatable<NetworkEvent<T>>
//         where T : unmanaged, IComparable, IConvertible, IComparable<T>, IEquatable<T> { // where T : Primitive types
//         public T Data;
//
//         public bool Equals(NetworkEvent<T> other) {
//             if (other == null) return false;
//
//             return Data.Equals(other.Data);
//         }
//
//         public void NetworkSerialize<T1>(BufferSerializer<T1> serializer) where T1 : IReaderWriter {
//             serializer.SerializeValue(ref Data);
//         }
//     }
//
//
//     public class UpArrowEvent : NetworkEvent<bool> { }
//
//     public class DownArrowEvent : NetworkEvent<bool> { }
//
//     public class RightArrowEvent : NetworkEvent<bool> { }
//
//     public class LeftArrowEvent : NetworkEvent<bool> { }
// }

