/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial cost objects                     Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information holder                      *
*  Type     : FinancialCostObjectEntry                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information for a financial cost object entry.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;


namespace Empiria.Financial.CostObject {

  /// <summary>Holds information for a financial cost objects entry.</summary>
  public class FinancialCostObjectEntry {

    public string ExternalCode {
      get; set;
    }
    
    public string Description {
      get; set;
    }

    public DateTime StartDate {
      get; set;
    } = DateTime.Today;

    public DateTime? EndDate {
      get; set;
    }

  } // Holds information for a financial cost object entry.

} // Empiria.Financial.CostObjects.Domain 