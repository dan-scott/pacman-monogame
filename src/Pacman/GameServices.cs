using Microsoft.Xna.Framework;

namespace Pacman
{
    public static class GameServices
    {
        private static GameServiceContainer _container;

        private static GameServiceContainer Instance => _container ?? (_container = new GameServiceContainer());

        public static T GetService<T>() where T : class => Instance.GetService<T>();
        public static void AddService<T>(T service) where T : class => Instance.AddService(service);
        public static void RemoveService<T>() where T : class => Instance.RemoveService(typeof(T));
    }
}