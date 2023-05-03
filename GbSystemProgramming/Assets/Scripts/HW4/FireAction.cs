using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class FireAction : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int startAmmunition = 20;
    protected string countBullet = string.Empty;
    public Queue<GameObject> Bullets { get; private set; } = new();
    protected Queue<GameObject> ammunition = new();
    protected bool reloading = false;

    protected virtual void Start()
    {
        for (var i = 0; i < startAmmunition; i++)
        {
            GameObject bullet;
            if (bulletPrefab == null)
            {
                bullet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bullet.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                bullet = Instantiate(bulletPrefab);
            }
            bullet.SetActive(false);
            ammunition.Enqueue(bullet);
        }
    }

    public virtual async void Reloading()
    {
        Bullets = await Reload();
    }

    protected virtual void Shooting()
    {
        if (Bullets.Count == 0)
        {
            Reloading();
        }
    }

    private async Task<Queue<GameObject>> Reload()
    {
        if (!reloading)
        {
            reloading = true;
            StartCoroutine(ReloadingAnim());
            return await Task.Run(delegate
            {
                var cage = 10;
                if (Bullets.Count < cage)
                {
                    Thread.Sleep(3000);
                    var bullets = this.Bullets;
                    while (bullets.Count > 0)
                    {
                        ammunition.Enqueue(bullets.Dequeue());
                    }
                    cage = Mathf.Min(cage, ammunition.Count);
                    if (cage > 0)
                    {
                        for (var i = 0; i < cage; i++)
                        {
                            var sphere = ammunition.Dequeue();
                            bullets.Enqueue(sphere);
                        }
                    }
                }
                reloading = false;
                return Bullets;
            });
        }
        else
        {
            return Bullets;
        }
    }

    private IEnumerator ReloadingAnim()
    {
        while (reloading)
        {
            countBullet = " | ";
            yield return new WaitForSeconds(0.01f);
            countBullet = @" \ ";
            yield return new WaitForSeconds(0.01f);
            countBullet = "---";
            yield return new WaitForSeconds(0.01f);
            countBullet = " / ";
            yield return new WaitForSeconds(0.01f);
        }
        countBullet = Bullets.Count.ToString();
        yield return null;
    }
}
