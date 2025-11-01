/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Ouput DTO                               *
*  Type     : StandardAccountDto                         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with data related to a standard account.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Financial.Adapters;

namespace Empiria.Financial.Concepts.Adapters {

  /// <summary>Output DTO holder for financial concepts.</summary>
  public class FinancialConceptHolder {

    public FinancialConceptDto Concept {
      get; internal set;
    }

    public FixedList<StandardAccountDescriptor> Integration {
      get; internal set;
    }

    public FinancialConceptActions Actions {
      get; internal set;
    }

  }  // class FinancialConceptHolder



  /// <summary>Actions DTO for a financial concept.</summary>
  public class FinancialConceptActions {

    public bool CanActivate {
      get; internal set;
    }

    public bool CanSuspend {
      get; internal set;
    }

    public bool CanUpdate {
      get; internal set;
    }

  }  // class FinancialConceptActions



  /// <summary>Output DTO for a financial concept.</summary>
  public class FinancialConceptDto {

    public string UID {
      get; internal set;
    }

    public string Number {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string FullName {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public NamedEntityDto Group {
      get; internal set;
    }

    public int Level {
      get; internal set;
    }

    public bool IsLastLevel {
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

  }  // class FinancialConceptDto



  /// <summary>Output DTO with an financial concept to be used in lists.</summary>
  public class FinancialConceptDescriptor {

    public string UID {
      get; internal set;
    }

    public string Number {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string FullName {
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

    public string StatusName {
      get; internal set;
    }

  }  // class FinancialConceptDescriptor



  public class FinancialConceptEntityDto {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string FullName {
      get; internal set;
    }

  }  // class FinancialConceptEntityDto



  public class FinancialConceptEntryDescriptor {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string TypeName {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string Operation {
      get; internal set;
    }

  }  // class FinancialConceptEntryDescriptor

}  // namespace Empiria.Financial.Concepts.Adapters
