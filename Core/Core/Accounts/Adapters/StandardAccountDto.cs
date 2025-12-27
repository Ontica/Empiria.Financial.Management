/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Accounts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Ouput DTO                               *
*  Type     : StandardAccountDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with data related to a standard account.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Adapters {

  public class StandardAccountHolder {

    public StandardAccountDto StandardAccount {
      get; internal set;
    }

    public FixedList<FinancialAccountDescriptor> Accounts {
      get; internal set;
    }

    public StandardAccountActions Actions {
      get; internal set;
    }

  }  // class StandardAccountHolder



  /// <summary>Actions DTO for standard accounts.</summary>
  public class StandardAccountActions {

    public bool CanActivate {
      get; internal set;
    }

    public bool CanSuspend {
      get; internal set;
    }

    public bool CanUpdate {
      get; internal set;
    }

    public bool CanEditOperations {
      get; internal set;
    }

    public bool ShowOperations {
      get; internal set;
    }

  }  // class StandardAccountActions



  /// <summary>Output DTO with an standard account data.</summary>
  public class StandardAccountDto {

    public string UID {
      get; internal set;
    }

    public string Number {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string FullName {
      get; internal set;
    }

    public NamedEntityDto Type {
      get; internal set;
    }

    public NamedEntityDto RoleType {
      get; internal set;
    }

    public NamedEntityDto DebtorCreditorType {
      get; internal set;
    }

    public NamedEntityDto Classification {
      get; internal set;
    }

    public int Level {
      get; internal set;
    }

    public bool IsLastLevel {
      get; internal set;
    }

    public bool IsProjectRelated {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

  }  // class StandardAccountDto



  /// <summary>Output DTO with an standard account data to be used in lists.</summary>
  public class StandardAccountDescriptor {

    public string UID {
      get; internal set;
    }

    public string Number {
      get; internal set;
    }

    [Newtonsoft.Json.JsonProperty("Name")]
    public string Description {
      get; internal set;
    }

    public string FullName {
      get; internal set;
    }

    public string TypeName {
      get; internal set;
    }

    public AccountRoleType RoleType {
      get; internal set;
    }

    public DebtorCreditorType DebtorCreditorType {
      get; internal set;
    }

    public int Level {
      get; internal set;
    }

    public bool IsLastLevel {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public bool Obsolete {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  }  // class StandardAccountDescriptor

}  // namespace Empiria.Financial.Adapters
