using JustChess.Data;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace JustChess.Project
{
    /// <summary>
    /// This container can be used to resolve project-wide dependencies from anywhere.
    /// Since resolving dependencies by hand from the container is actually an anti-pattern,
    /// use this reference only when absolutely needed
    /// </summary>
    public static class ProjectDiContainer
    {
        public static DiContainer Container;
    }

    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ColorPalette colorPalette;
        [SerializeField] private PieceSpriteSet pieceSpriteSet;
        [SerializeField] private MainSettings mainSettings;
        
        public override void InstallBindings()
        {
            ProjectDiContainer.Container = Container;
            
            Container.BindInstance(colorPalette).AsSingle();
            Container.BindInstance(pieceSpriteSet).AsSingle();
            Container.BindInstance(mainSettings).AsSingle();
        }
    }
}