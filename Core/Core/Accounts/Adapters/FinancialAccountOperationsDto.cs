/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Output DTO                              *
*  Type     : FinancialAccountOperationsDto              License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with information for an account and its available and current operations.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Output DTO with information for an account and its available and current operations.</summary>
  public class FinancialAccountOperationsDto {

    public FinancialAccountDescriptor Account {
      get; internal set;
    }

    public FixedList<NamedEntityDto> AvailableOperations {
      get; internal set;
    }

    public FixedList<NamedEntityDto> CurrentOperations {
      get; internal set;
    }

  }  // class FinancialAccountOperationsDto

}  // namespace Empiria.Financial.Adapters
