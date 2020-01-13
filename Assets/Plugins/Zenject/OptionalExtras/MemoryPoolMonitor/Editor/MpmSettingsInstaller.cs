using UnityEngine;
using UnityEditor;

namespace Zenject.MemoryPoolMonitor
{
    [CreateAssetMenu(fileName = "MpmSettingsInstaller", menuName = "Installers/MpmSettingsInstaller")]
    public class MpmSettingsInstaller : ScriptableObjectInstaller<MpmSettingsInstaller>
    {
        public MpmView.Settings MpmView;
        public MpmView.Settings MpmViewDark;

        public override void InstallBindings()
        {
            Container.BindInstance(EditorGUIUtility.isProSkin ? MpmViewDark : MpmView);
        }
    }
}
