/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Adaptation Interface                    *
*  Type     : IOrganizationUnitData                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface with organization unit data coming from external systems.                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;

namespace Empiria.Financial.Adapters {

  /// <summary>Interface with organization unit data coming from external systems.</summary>
  public interface ISialOrganizationUnitData {

    string OrganizationUnitNo {
      get;
    }

    string OrganizationUnitName {
      get;
    }

    string SupervisingUnitNo {
      get;
    }

    string SupervisingUnitName { 
      get; 
    }

    int HierarchicalLevel {
      get;
    }


  }  // IOrganizationUnitData

} // namespace Empiria.Financial.Adapters
