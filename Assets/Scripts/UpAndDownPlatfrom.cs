using UnityEngine;

/*
Moves platform back and forth between two Y positions.
Inherits from Freezable.cs to allow freezing.
*/

public class UpAndDownPlatform : Freezable
{
    [Header("World Y positions to travel between")]
    public float yA = 5f;   // 下边界
    public float yB = 10f;   // 上边界

    [Header("Motion")]
    public float speed = 10f;
    public float waitAtEnds = 0.25f;   // 到达端点时的等待时间

    private int dir = 1;          // +1 表示往 yB 移动，-1 表示往 yA 移动
    private float waitTimer = 0f; // 端点等待计时器

    void Start()
    {
        var p = transform.position;
        float dA = Mathf.Abs(p.y - yA);
        float dB = Mathf.Abs(p.y - yB);

        // 开始时吸附到最近的端点 (yA 或 yB)
        transform.position = new Vector3(p.x, dA <= dB ? yA : yB, p.z);

        // 初始方向：如果在 yA 就往上，否则往下
        dir = (Mathf.Abs(transform.position.y - yA) < 0.001f) ? +1 : -1;
    }

    protected override void Update()
    {
        base.Update(); // 保持冻结逻辑

        if (IsFrozen)
        {
            return;
        }

        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        float targetY = (dir > 0) ? yB : yA;
        var pos = transform.position;

        // 移动到目标 Y
        float newY = Mathf.MoveTowards(pos.y, targetY, speed * Time.deltaTime);
        transform.position = new Vector3(pos.x, newY, pos.z);

        // ⚡ 阈值判断（0.001f 精度足够）
        if (Mathf.Abs(newY - targetY) <= 0.001f)
        {
            dir *= -1;
            waitTimer = waitAtEnds;
        }
    }

    protected override void OnFreeze()
    {
        base.OnFreeze();
        var lockComponent = GetComponent<FreezeTransformLock>();
        if (lockComponent)
        {
            lockComponent.SnapshotNow(); // 冻结时立即锁定位置，避免抖动
        }
    }
}
