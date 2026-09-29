/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                            Component : Adapters Layer                     *
*  Assembly : Empiria.Financial.Integration.Core.dll          Pattern   : Data Transfer Object               *
*  Type     : AccountingClassificationDto                     License   : Please read LICENSE.txt file       *
*                                                                                                            *
*  Summary  : Output DTOs with data related to an accounting classification.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/


namespace Empiria.FinancialAccounting.Transactions.Adapters {

  /// <summary> Output DTOs with data related to an accounting classification.</summary>
  public class AccountingClassificationDto {

    public string SelectorValue {
      get; set;
    }

    public string Name {
      get; set;
    }

    public FixedList<NamedEntityDto> Classifications {
      get; set;
    }

  }  // class AccountingClassificationDto

}  // namespace Empiria.FinancialAccounting.Transactions.Adapters
