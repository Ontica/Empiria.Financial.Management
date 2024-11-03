/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : External Services Layer                 *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Service connector                       *
*  Type     : BaseActions                                License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Connect Empiria Payments with external services providers.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments {

  // <summary>Output DTO used to return actions</summary>
  public class BaseActions {

    public bool CanDelete {
      get; internal set;
    }

    public bool CanEditDocuments {
      get; internal set;
    }

    public bool CanUpdate {
      get; internal set;
    }

  } // class BaseActions

}  // namespace Empiria.Payments
