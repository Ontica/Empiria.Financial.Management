/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments validator services                Component : payment notfication                     *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Payment validator processor             *
*  Type     : PaymentsProcessor                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services to process payment instructions.                                             *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using System.Threading;

namespace Empiria.Payments.Processor {

  /// <summary>Provides services to process payment instructions.</summary>
  public class PaymentsProcessor {

    private static bool isRunning = false;
    private static volatile Timer timer = null;

    #region Properties

    static public bool IsRunning {
      get {
        return isRunning;
      }
    }

    #endregion Properties

    #region Methods

    /// <summary>Starts the execution engine for pending messages in asyncronous mode.</summary>
    static public void Start() {
      try {

        if (isRunning) {
          return;
        }

        isRunning = true;

        int MESSAGE_ENGINE_EXECUTION_MINUTES = ConfigurationData.Get("MessageEngine.Execution.Minutes", 1);

        timer = new Timer(ProcessPaymentInstructions, null,
                          TimeSpan.FromSeconds(MESSAGE_ENGINE_EXECUTION_MINUTES),
                          TimeSpan.FromMinutes(MESSAGE_ENGINE_EXECUTION_MINUTES));

        EmpiriaLog.Info("PaymentsProcessor was started.");

      } catch (Exception e) {
        EmpiriaLog.Info("PaymentsProcessor was stopped due to an ocurred exception.");

        EmpiriaLog.Error(e);

        isRunning = false;
      }
    }


    /// <summary>Stops the execution engine.</summary>
    static public void Stop() {
      if (!isRunning) {
        return;
      }
      timer.Dispose();
      timer = null;
      isRunning = false;

      EmpiriaLog.Info("PaymentsProcessor was stopped.");
    }


    #endregion Methods

    #region Helpers

    static private async void ProcessPaymentInstructions(object stateInfo) {

      var toBeRequested = PaymentInstruction.GetReadyToBeRequested();

      var inProgress = PaymentInstruction.GetInProgress();

      using (var usecases = PaymentService.ServiceInteractor()) {

        foreach (var instruction in toBeRequested) {
          await usecases.SendPaymentInstruction(instruction);
        }

        foreach (var instruction in inProgress) {
          await usecases.RefreshPaymentInstruction(instruction);
        }
      }
    }

    #endregion Helpers

  }  // class PaymentsExecutor

} // namespace Empiria.Payments.Processor
