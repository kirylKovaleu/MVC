using Auction.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Configure
{
    public class GetAuctioneHouse
    {
        public List<SelectListItem> GetHouses()
        {
            HousesConfigSection section = (HousesConfigSection)ConfigurationManager.GetSection("Houses");
            HousesCollection d = section.Houses;
            List<SelectListItem> result = new List<SelectListItem>();

            for (int i = 0; i < d.Count; i++)
            {
                var ittem = new SelectListItem
                {
                    Text = d[i].Name,
                    Value = d[i].Name
                };
                result.Add(ittem);
            }
            return result;
        }

        public AuctionModel GetPath(string name)
        {
            HousesConfigSection section = (HousesConfigSection)ConfigurationManager.GetSection("Houses");
            HousesCollection d = section.Houses;
            AuctionModel auctionItem = new AuctionModel();

            for (int i = 0; i < d.Count; i++)
            {
                if (d[i].Name == name)
                {
                    auctionItem.Name = name;
                    auctionItem.Type = d[i].Type;
                    auctionItem.Path = d[i].Path;
                }
            }
            return auctionItem;
        }
    }
}
