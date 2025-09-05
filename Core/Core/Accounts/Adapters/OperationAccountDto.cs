/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                                Component : Adapters Layer                   *
*  Assembly : Empiria.Financial.Core.dll                        Pattern   : Output DTO                       *
*  Type     : OperationAccountsHolderDto, OperationAccountDto   License   : Please read LICENSE.txt file     *
*                                                                                                            *
*  Summary  : Output DTO with information for an account and its available and current operations.           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {


  /// <summary>Output DTO with am an account and its available and current operations for edition.</summary>
  public class OperationAccountsStructure {

    public FinancialAccountDescriptor BaseAccount {
      get; internal set;
    }

    public FixedList<NamedEntityDto> AvailableOperations {
      get; internal set;
    }

    public FixedList<OperationAccountDto> CurrentOperations {
      get; internal set;
    }

  }  // class OperationAccountsStructure



  /// <summary>Output DTO for an operation account.</summary>
  public class OperationAccountDto {

    public string UID {
      get; internal set;
    }

    public string AccountNo {
      get; internal set;
    }

    public string OperationTypeName {
      get; internal set;
    }

    public string CurrencyName {
      get; internal set;
    }

  }  // class OperationAccountDto

}  // namespace Empiria.Financial.Adapters
