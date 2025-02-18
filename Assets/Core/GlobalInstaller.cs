﻿using StarterCore.Core.Services.Network;
using UnityEngine;
using Zenject;
using StarterCore.Core.Services.Navigation;
using StarterCore.Core.Services.GameState;
using StarterCore.Core.Services.Localization;


namespace StarterCore.Core
{
    [CreateAssetMenu(fileName = "GlobalInstaller", menuName = "StarterCore/GlobalInstaller", order = 0)]

    public class GlobalInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private NavigationSetup _navSetup;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EntityDeckService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PropertyDeckService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LocalizationManager>().AsSingle().NonLazy();

            Container.Bind<NetworkService>().AsSingle().NonLazy();
            Container.Bind<APIService>().AsSingle().NonLazy();

            Container.Bind<NavigationSetup>().FromInstance(_navSetup);
            Container.BindInterfacesAndSelfTo<NavigationService>().AsSingle().NonLazy();
        }
    }
}

