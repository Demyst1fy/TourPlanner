namespace TourPlanner.BusinessLayer
{
    public static class TourHandlerSingleton {
        private static ITourHandler handler;

        public static ITourHandler GetHandler() {
            if (handler == null) {
                handler = new TourHandler();
            }
            return handler;
        }
    }
}
