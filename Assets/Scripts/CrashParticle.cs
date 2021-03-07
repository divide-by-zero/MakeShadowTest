using UnityEngine;

public class CrashParticle : PoolMonoBehaviour<CrashParticle>
{
    public Color? Color { set; get; }
    private ParticleSystem ps;
    private ParticleSystem.Particle[] particles;

    public void CreateParticle(float w,float h,float size,Color color,float? step = null)
    {
        if (step.HasValue == false) step = size;
        //すでにParticleSystemがComponentにある場合はそれを使う。ない場合は追加
        if (TryGetComponent(out ps) == false)
        {
            ps = gameObject.AddComponent<ParticleSystem>();
        }
        // ps.hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
        int particleSize = Mathf.CeilToInt(w/step.Value) * Mathf.CeilToInt(h/step.Value);
        var main = ps.main;
        main.loop = false;
        main.gravityModifier = 1.0f;
        main.playOnAwake = false;
        main.maxParticles = particleSize;
        main.startLifetime = 5.0f;

        ps.Emit(particleSize);
        particles = new ParticleSystem.Particle[particleSize];
        ps.GetParticles(particles);

        int particleCount = 0;
        for (var y = 0.0f; y < h; y+=step.Value)
        {
            for (var x = 0.0f; x < w; x+=step.Value)
            {
                Vector3 targetPos = new Vector3(x-w/2.0f, y-h/2.0f, 0);
                ParticleSystem.Particle p = particles[particleCount];
                p.startLifetime = 5.0f;
                p.position = targetPos;
                p.rotation = Random.Range(0.0f, 360.0f);
                p.size = Random.Range(size * 0.9f, size * 1.1f);    //パーティクルのサイズ設定
                p.startColor = color;    //TODO 色はどうしようね
                // p.velocity = Vector3.zero;
                p.velocity = Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(0, Random.Range(0.6f, 1.0f), 0);    //ちょっと散らばらせる？
                particles[particleCount] = p;
                particleCount++;
            }
        }
        ps.SetParticles(particles, particleCount);
    }

    void Update ()
    {
        if (particles == null) return;
        //全部のパーティクルの寿命が尽きたら消える
        int num = ps.GetParticles(particles);
        if (num <= 0)
        {
            CrashParticle.PoolDestroy(this);
        }
    }
}