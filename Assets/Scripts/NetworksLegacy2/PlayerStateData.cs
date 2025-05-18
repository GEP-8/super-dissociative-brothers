using Unity.Netcode;

// SerializeField를 사용하려면 필요하지만, 이 구조체 자체에는 필요 없음

// PlayerStateData는 struct로 변경하여 값 타입으로 사용, INetworkSerializable 구현
public struct PlayerStateData : INetworkSerializable {
    public float rightMoveSpeed;
    public float leftMoveSpeed;
    public float upMoveSpeed;
    public float downMoveSpeed;
    public bool isJumping;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref rightMoveSpeed);
        serializer.SerializeValue(ref leftMoveSpeed);
        serializer.SerializeValue(ref upMoveSpeed);
        serializer.SerializeValue(ref downMoveSpeed);
        serializer.SerializeValue(ref isJumping);
    }

    public PlayerStateData(float r = 0, float l = 0, float u = 0, float d = 0, bool j = false) {
        rightMoveSpeed = r;
        leftMoveSpeed = l;
        upMoveSpeed = u;
        downMoveSpeed = d;
        isJumping = j;
    }
}

// 어떤 상태 필드를 제어할 수 있는지 명시하기 위한 열거형
public enum ControllableStateField {
    RightMoveSpeed,
    LeftMoveSpeed,
    UpMoveSpeed,
    DownMoveSpeed,
    IsJumping
}