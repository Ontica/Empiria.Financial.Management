/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Adaptation Interface                    *
*  Type     : IOrganizationUnitEmployeesData             License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface with organization unit employees data coming from external systems.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Interface with organization unit employees data coming from external systems.</summary>
  public interface ISialOrganizationUnitEmployeesData {

    string EmployeeNo {
      get;
    }

    string FedTaxpayersReg {
      get;
    }

    string Name {
      get;
    }

    string LastName {
      get;
    }

    string OrganizationUnitNo {
      get;
    }

    string JobNo {
      get;
    }

    string JobTitle { 
      get; 
    }

    string JobCategoryNo {
      get;
    }

  }  // ISialOrganizationUnitEmployeesData

} // namespace Empiria.Financial.Adapters
