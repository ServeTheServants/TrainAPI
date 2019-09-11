using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TrainAPI.Models;

namespace TrainAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly TrainContext db;

        public TrainsController(TrainContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public ActionResult<List<Train>> GetTrains(string dispatchingStation, string arrivingStation, int day)
        {
            /* Заполненные данные:
            var train1 = new Train("752A");
            var train2 = new Train("754A");
            var train3 = new Train("756A");
            var train4 = new Train("760A");
            var train5 = new Train("768A");
            var train6 = new Train("770A");
            var train7 = new Train("772A");
            var train8 = new Train("774A");
            var train9 = new Train("776A");
            var train10 = new Train("778A");
            var train11 = new Train("780A");

            var station1 = new Station("Москва");
            var station2 = new Station("Санкт-Петербург");
            var station3 = new Station("Тверь");
            var station4 = new Station("Бологое");
            var station5 = new Station("Вышний Волочек");
            var station6 = new Station("Угловка");
            var station7 = new Station("Окуловка");
            var station8 = new Station("Чудово");

            var route1 = new Route(train1);
            route1.Schedules = new List<Schedule>()
            {
                new Schedule(route1, station1, 1, null, null, null, 1, 5, 45),
                new Schedule(route1, station2, 2, 1, 9, 29, null, null, null)
            };
            train1.Routes = new List<Route>() { route1 };

            var route2 = new Route(train2);
            route2.Schedules = new List<Schedule>()
            {
                new Schedule(route2, station1, 1, null, null, null, 1, 6, 45),
                new Schedule(route2, station3, 2, 1, 7, 50, 1, 7, 51),
                new Schedule(route2, station4, 3, 1, 8, 47, 1, 8, 48),
                new Schedule(route2, station2, 4, 1, 10, 50, null, null, null)
            };
            train2.Routes = new List<Route>() { route2 };

            var route3 = new Route(train3);
            route3.Schedules = new List<Schedule>()
            {
                new Schedule(route3, station1, 1, null, null, null, 1, 7, 0),
                new Schedule(route3, station3, 2, 1, 8, 2, 1, 8, 3),
                new Schedule(route3, station5, 3, 1, 8, 43, 1, 8, 44),
                new Schedule(route3, station6, 4, 1, 9, 18, 1, 9, 19),
                new Schedule(route3, station7, 5, 1, 9, 28, 1, 9, 29),
                new Schedule(route3, station8, 6, 1, 10, 10, 1, 10, 11),
                new Schedule(route3, station2, 7, 1, 11, 10, null, null, null)
            };
            train3.Routes = new List<Route>() { route3 };

            var route4 = new Route(train4);
            route4.Schedules = new List<Schedule>()
            {
                new Schedule(route4, station1, 1, null, null, null, 1, 9, 40),
                new Schedule(route4, station5, 2, 1, 11, 21, 1, 11, 22),
                new Schedule(route4, station4, 3, 1, 11, 39, 1, 11, 40),
                new Schedule(route4, station6, 4, 1, 11, 59, 1, 12, 0),
                new Schedule(route4, station2, 5, 1, 13, 43, null, null, null)
            };
            train4.Routes = new List<Route>() { route4 };

            var route5 = new Route(train5);
            route5.Schedules = new List<Schedule>()
            {
                new Schedule(route5, station1, 1, null, null, null, 1, 16, 5),
                new Schedule(route5, station4, 2, 1, 17, 58, 1, 17, 59),
                new Schedule(route5, station8, 3, 1, 18, 59, 1, 19, 0),
                new Schedule(route5, station2, 4, 1, 19, 58, null, null, null)
            };
            train5.Routes = new List<Route>() { route5 };

            var route6 = new Route(train6);
            route6.Schedules = new List<Schedule>()
            {
                new Schedule(route6, station1, 1, null, null, null, 1, 16, 15),
                new Schedule(route6, station3, 2, 1, 17, 17, 1, 17, 18),
                new Schedule(route6, station5, 3, 1, 17, 58, 1, 17, 59),
                new Schedule(route6, station6, 4, 1, 18, 33, 1, 18, 34),
                new Schedule(route6, station2, 5, 1, 20, 15, null, null, null)
            };
            train6.Routes = new List<Route>() { route6 };

            var route7 = new Route(train7);
            route7.Schedules = new List<Schedule>()
            {
                new Schedule(route7, station1, 1, null, null, null, 1, 17, 30),
                new Schedule(route7, station4, 2, 1, 19, 27, 1, 19, 28),
                new Schedule(route7, station8, 3, 1, 20, 28, 1, 20, 29),
                new Schedule(route7, station2, 4, 1, 21, 27, null, null, null)
            };
            train7.Routes = new List<Route>() { route7 };

            var route8 = new Route(train8);
            route8.Schedules = new List<Schedule>()
            {
                new Schedule(route8, station1, 1, null, null, null, 1, 17, 40),
                new Schedule(route8, station3, 2, 1, 18, 45, 1, 18, 46),
                new Schedule(route8, station4, 3, 1, 19, 42, 1, 19, 43),
                new Schedule(route8, station2, 4, 1, 21, 48, null, null, null)
            };
            train8.Routes = new List<Route>() { route8 };

            var route9 = new Route(train9);
            route9.Schedules = new List<Schedule>()
            {
                new Schedule(route9, station1, 1, null, null, null, 1, 19, 30),
                new Schedule(route9, station3, 2, 1, 20, 35, 1, 20, 36),
                new Schedule(route9, station4, 3, 1, 21, 32, 1, 21, 33),
                new Schedule(route9, station2, 4, 1, 23, 35, null, null, null)
            };
            train9.Routes = new List<Route>() { route9 };

            var route10 = new Route(train10);
            route10.Schedules = new List<Schedule>()
            {
                new Schedule(route10, station1, 1, null, null, null, 1, 19, 45),
                new Schedule(route10, station3, 2, 1, 20, 47, 1, 20, 48),
                new Schedule(route10, station5, 3, 1, 21, 28, 1, 21, 29),
                new Schedule(route10, station6, 4, 1, 22, 3, 1, 22, 4),
                new Schedule(route10, station7, 5, 1, 22, 13, 1, 22, 14),
                new Schedule(route10, station8, 6, 1, 22, 55, 1, 22, 56),
                new Schedule(route10, station2, 7, 1, 23, 54, null, null, null)
            };
            train10.Routes = new List<Route>() { route10 };

            var route11 = new Route(train11);
            route11.Schedules = new List<Schedule>()
            {
                new Schedule(route11, station1, 1, null, null, null, 1, 21, 0),
                new Schedule(route11, station3, 2, 1, 22, 2, 1, 22, 3),
                new Schedule(route11, station2, 3, 2, 0, 53, null, null, null)
            };
            train11.Routes = new List<Route>() { route11 };

            db.Trains.AddRange(train1, train2, train3, train4, train5, train6, train7, train8, train9, train10, train11);
            db.SaveChanges();
            */

            var station11 = db.Stations.Where(station => station.Station_name == dispatchingStation).FirstOrDefault();
            if (station11 == null)
            {
                return NotFound("Данной станции отправления не существует");
            }

            var station12 = db.Stations.Where(station => station.Station_name == arrivingStation).FirstOrDefault();
            if (station12 == null)
            {
                return NotFound("Данной станции прибытия отправления не существует");
            }

            //var trains_with_includes = db.Trains.Include(train => train.Routes).ThenInclude(route => route.Schedules).ThenInclude(schedule => schedule.Station); - если надо со всеми данными
            var trains = (from train in db.Trains
                          from route in train.Routes
                          let schedule1 = route.Schedules.FirstOrDefault(schedule => schedule.StationId == station11.StationId && schedule.DispatchingDay == day)
                          let schedule2 = route.Schedules.FirstOrDefault(schedule => schedule.StationId == station12.StationId)
                          where schedule1 != null && schedule2 != null
                          where schedule1.StationNumberInRoute < schedule2.StationNumberInRoute
                          select train).ToList();

            if (trains.Count == 0)
                return NotFound("Поездов по данному маршруту в этот день нет");
            else
                return Ok(trains);
        }
    }
}
