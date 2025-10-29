/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information holder                      *
*  Type     : FinancialConceptEntry                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Holds information for a financial concept entry.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

namespace Empiria.Financial.Concepts {

  /// <summary>Holds information for a financial concept entry.</summary>
  internal class FinancialConceptEntry : BaseObject {

    public string Name {
      get; private set;
    }

    public string Operation {
      get; private set;
    }

    public string TypeName {
      get; private set;
    }

    public DateTime StartDate {
      get; private set;
    }

    public DateTime EndDate {
      get; private set;
    }

  }  // class FinancialConceptEntry

}   // namespace Empiria.Financial.Concepts
