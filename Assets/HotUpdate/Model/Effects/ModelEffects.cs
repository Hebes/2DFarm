
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
using Core;
using Debug = Core.Debug;

namespace Farm2D
{
    public class ModelEffects : IModelInit
    {
        public static ModelEffects Instanck;
        public async UniTask ModelInit()
        {
            Instanck = this;
            ConfigEvent.ParticleEffect.EventAdd<EParticleEffectType, Vector3>(OnParticleEffectEvent);
            await UniTask.Yield();
        }

        private void OnParticleEffectEvent(EParticleEffectType effectType, Vector3 pos)
        {
            switch (effectType)
            {
                default:
                case EParticleEffectType.None:
                    break;
                case EParticleEffectType.LeavesFalling01:
                case EParticleEffectType.LeavesFalling02:
                case EParticleEffectType.RockEffect:
                case EParticleEffectType.GrassEffect:
                    CorePool.Instance.GetObj(effectType.ToString(), (obj) =>
                    {
                        obj.transform.position = pos;
                        ReleaseRoutine(obj).Forget();
                    });
                    break;
            }
        }

        private async UniTask ReleaseRoutine(GameObject obj)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), ignoreTimeScale: false);
            Debug.Log($"当前物体名称是{obj.name}");
            CorePool.Instance.PushObj(obj.name, obj);
        }
    }
}
