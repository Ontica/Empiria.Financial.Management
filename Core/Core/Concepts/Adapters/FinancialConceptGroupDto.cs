/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Ouput DTO                               *
*  Type     : FinancialConceptGroupDto                   License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Output DTO with data related to a financial concepts group.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Concepts.Adapters {

  /// <summary>Output DTO with data related to a financial concepts group.</summary>
  public class FinancialConceptGroupDto {

    internal FinancialConceptGroupDto() {
      // no-op
    }

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string NamedKey {
      get; internal set;
    }

    public FixedList<FinancialConceptDescriptor> Concepts {
      get; internal set;
    }

  }  // class FinancialConceptGroupDto



  /// <summary>Output DTO for a financial concept group.</summary>
  public class FinancialConceptGroupDescriptor {

    public string UID {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string NamedKey {
      get; internal set;
    }

    public bool IsReadOnly {
      get; internal set;
    }

  }  // class FinancialConceptGroupDescriptor

}  // namespace Empiria.Financial.Concepts.Adapters
