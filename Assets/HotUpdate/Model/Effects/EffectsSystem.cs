
/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	特效系统

-----------------------*/


using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace ACFrameworkCore
{
    public class EffectsSystem : ICore
    {
        public void ICroeInit()
        {
            ConfigEvent.ParticleEffect.AddEventListener<EParticleEffectType, Vector3>(OnParticleEffectEvent);
        }

        private void OnParticleEffectEvent(EParticleEffectType effectType, Vector3 pos)
        {
            switch (effectType)
            {
                case EParticleEffectType.None:
                    break;
                case EParticleEffectType.LeavesFalling01:
                case EParticleEffectType.LeavesFalling02:
                    PoolManager.Instance.GetObj(effectType.ToString(), (obj) =>
                    {
                        obj.transform.position = pos;
                        ReleaseRoutine(obj).Forget();
                    });
                    break;
                case EParticleEffectType.RockEffect:
                    break;
                case EParticleEffectType.ReapableScenery:
                    break;
                default:
                    break;
            }
        }

        private async UniTask ReleaseRoutine(GameObject obj)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), ignoreTimeScale: false);
            //yield return new WaitForSeconds(seconds: 1.5f);
            ACDebug.Log($"当前物体名称是{obj.name}");
            PoolManager.Instance.PushObj(obj.name, obj);
        }
    }
}
