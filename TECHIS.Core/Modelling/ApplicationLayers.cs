using System;
using System.Collections.Generic;
using System.Text;

namespace TECHIS.Core.Modelling
{
    public enum ApplicationLayers
    {
        None,
        DataLayer,
        DataAccessLayer,
        BusinessLayer,
        BusinessLayerEntityModel,
        BusinessLayerCache,
        BusinessLayerBusinessService,
        WebService,
        WebServiceServiceMapping,
        WebServiceContract,
        UI,
        UIProcessComponent,
        UIComponent
    }
}
