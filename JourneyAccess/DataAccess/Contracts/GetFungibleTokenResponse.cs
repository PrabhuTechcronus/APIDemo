using System;
using System.Collections.Generic;
using System.Text;
using Core;
using UG.Journey.JourneyAccess.DataAccess.Models;

namespace UG.Journey.JourneyAccess.DataAccess.Contracts
{
   public class GetFungibleTokenResponse
    {
        public ValidationResults validationResult { get; set; }
        public FungibleTokenTbl fungibleToken { get; set; }
    }
}
