/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : CashFlow Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Output DTOs                             *
*  Type     : CashFlowProjectionAccountDto               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO used to return cash flow projection accounts.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.CashFlow.Projections.Adapters {


  /// <summary>Output DTO used to return cash flow projection accounts.</summary>
  public class CashFlowProjectionAccountDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Currencies {
      get; internal set;
    }

  }  // class CashFlowProjectionAccountDto

}  // namespace Empiria.CashFlow.Projections.Adapters
