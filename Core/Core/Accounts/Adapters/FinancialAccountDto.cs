/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                           Component : Use Cases Layer                       *
*  Assembly : Empiria.Financial.Core.dll                   Pattern   : Use cases                             *
*  Type     : FinancialAccountDto                          License   : Please read LICENSE.txt file          *
*                                                                                                            *
*  Summary  : Provides use cases for update and retrieve financial accounts.                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents;
using Empiria.History;

using Empiria.Financial.Projects.Adapters;

namespace Empiria.Financial.Adapters {

  /// <summary>Holder output DTO used to return a financial account</summary>
  public class FinancialAccountHolderDto {

    public FinancialAccountDto Account {
      get; internal set;
    }

    public FinancialProjectDto Project {
      get; internal set;
    }

    public OperationAccountsStructure OperationAccounts {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryEntryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }

  }  // class FinancialAccountHolderDto


  public class FinancialAccountDto {

    public string UID {
      get; internal set;
    }

    public string AccountNo {
      get; internal set;
    }

    public NamedEntityDto FinancialAccountType {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto StandardAccount {
      get; internal set;
    }

    public NamedEntityDto OrganizationalUnit {
      get; internal set;
    }

    public NamedEntityDto Currency {
      get; internal set;
    }

    public string SubledgerAccountNo {
      get; internal set;
    }

    public NamedEntityDto Project {
      get; internal set;
    }

    public AccountAttributes Attributes {
      get; internal set;
    }

    public FinancialData FinancialData {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public NamedEntityDto Parent {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  } // class FinancialAccountDto



  public class FinancialAccountDescriptor {

    public string UID {
      get; internal set;
    }

    public string AccountNo {
      get; internal set;
    }

    public string FinancialAccountTypeName {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string StandardAccountName {
      get; internal set;
    }

    public string ProjectUID {
      get; internal set;
    }

    public string ProjectNo {
      get; internal set;
    }

    public string ProjectName {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty(PropertyName = "ProjectTypeName")]
    public string ProjectCategoryName {
      get; internal set;
    }

    public string OrganizationalUnitCode {
      get; internal set;
    }

    public string OrganizationalUnitName {
      get; internal set;
    }

    public string CurrencyName {
      get; internal set;
    }

    public string SubledgerAccountNo {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

    public AccountAttributes Attributes {
      get; internal set;
    }

    public FinancialData FinancialData {
      get; internal set;
    }

  }  // class FinancialAccountDescriptor

} // namespace Empiria.Financial.Adapters
