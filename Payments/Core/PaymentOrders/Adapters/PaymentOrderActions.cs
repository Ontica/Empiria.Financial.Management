/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Data Transfer Object                    *
*  Type     : PaymentOrderActions                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Class used to set payment order actions.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Payments.Adapters {

  /// <summary>Class used to set payment order actions.</summary>
  public class PaymentOrderActions : BaseActions {

    #region Constructors and parsers

    static private PaymentOrderActions _actions = new PaymentOrderActions();

    private PaymentOrderActions() {
    }

    #endregion Constructors and parsers

    #region Properties

    public bool CanSendToPay {
      get; internal set;
    }

    #endregion Properties

    #region Public Methods

    static internal PaymentOrderActions SetActions(PaymentOrderStatus status) {

      switch (status) {
        case PaymentOrderStatus.Pending: {
          SetPendingActions();
        }
        break;
        default: {
          SetDeletedActions();
        }
        break;
      }

      return _actions;
    }

    #endregion Public Methods

    #region Helpers

    static private void SetPendingActions() {
      _actions.CanDelete = true;
      _actions.CanSendToPay = true;
      _actions.CanUpdate = true;
      _actions.CanEditDocuments = true;
    }

    static private void SetDeletedActions() {
      _actions.CanDelete = false;
      _actions.CanSendToPay = false;
      _actions.CanUpdate = false;
      _actions.CanEditDocuments = false;
    }

    #endregion Helpers

  } // class PaymentOrderActions

}  // namespace Empiria.Payments.Adapters
