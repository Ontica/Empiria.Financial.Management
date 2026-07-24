/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Cost Object                        Component : Adapters Layer                        *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Ouput DTO                             *
*  Type     : CostObjectDto                                License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Output DTO for a cost objects.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.CostObject.Adapters {

  /// <summary>Output DTO for a cost objects.</summary>
  public class FinancialCostObjectDto {

    public string UID {
      get; internal set;
    }

    public int CostObjectTypeId {
     get; internal set;
    }

    public string ExternalCode {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string Status {
      get; internal set;
    }

  }   // class FinancialCostObjectDto

} //  Empiria.Financial.CostObject.Adapters
