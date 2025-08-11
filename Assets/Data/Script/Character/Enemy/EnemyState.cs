public enum EnemyState
{
    Idle, //待机
    Patrol, //巡逻
    Chase, //追踪
    Attack, //攻击
    KnockBack, //受击(被击退)
    KnockUp,//受击(被击飞)
    Dizzy, //眩晕状态
    Death, //死亡状态
    Vectory, //胜利：玩家死亡时
}
