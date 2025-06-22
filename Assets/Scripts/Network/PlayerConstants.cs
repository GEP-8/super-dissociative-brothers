using System;

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
        Jumping
    }

    public enum MergeStrategy {
        Latest
    }

    public class PlayerConstants {
       public static PlayerAction[][][] keyDistribution = new[] {
            // 0명일때
            new[] {
                Array.Empty<PlayerAction>()
            },
            // 1명일때
            new[] {
                new[] {
                    PlayerAction.RightMove,
                    PlayerAction.LeftMove,
                    PlayerAction.Jump,
                    PlayerAction.Crouch
                }
            },
            // 2명일때
            new[] {
                new[] { // player 1
                    PlayerAction.RightMove,
                    PlayerAction.LeftMove,
                },
                new[] { // player 2
                    PlayerAction.Jump,
                    PlayerAction.Crouch
                }
            },
            // 3명일때
            new[] {
                new[] { // player 1
                    PlayerAction.RightMove,
                    PlayerAction.LeftMove,
                },
                new[] { // player 2
                    PlayerAction.Jump,
                },
                new[] { // player 3
                    PlayerAction.Crouch
                }
            },
            // 4명일때
            new[] {
                new[] { // player 1
                    PlayerAction.RightMove,
                },
                new[] { // player 2
                    PlayerAction.LeftMove,
                },
                new[] { // player 3
                    PlayerAction.Jump,
                },
                new[] { // player 4
                    PlayerAction.Crouch
                }
            },
        };
    }
}
