using Student_manage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_manage.ViewModels
{
    public class StateCityViewModel
    {
        public State_Master State { get; set; } = new State_Master();
        public List<City_Master> Cities { get; set; } = new List<City_Master>();

        public StateCityViewModel()
        {
            if (State.Cities == null)
                State.Cities = new List<City_Master>();
        }
    }

}