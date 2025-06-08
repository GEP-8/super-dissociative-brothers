namespace Network {
    public enum PlayerAction
    {
        RightMove,
        LeftMove,
        // UpMove,
        // DownMove,
        Jump,
        Crouch
    }
    

    public enum JumpState
    {
        Idle,
        JumpUp,
        JumpDown
    }

    public enum MergeStrategy {
        Latest
    }
}