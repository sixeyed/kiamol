const log = require("./log");

function post(req, res, next) {
  log.Logger.debug("** POST /validate called");

  var admissionRequest = req.body;
  var object = admissionRequest.request.object;
  log.Logger.info(`Validating object; request UID: ${admissionRequest.request.uid}`);

  var admissionResponse = {
    uid: admissionRequest.request.uid,
    allowed: false
  };

  if (object.spec.hasOwnProperty("automountServiceAccountToken")) {
    admissionResponse.allowed = (object.spec.automountServiceAccountToken == false);
  }
  else {
    log.Logger.info("- no automountServiceAccountToken");
  }

  if (!admissionResponse.allowed) {
    admissionResponse.status = {
      status: 'Failure',
      message: "automountServiceAccountToken must be set to false",
      reason: "automountServiceAccountToken must be set to false",
      code: 400
    }
  }

  var admissionReview = {
    apiVersion: admissionRequest.apiVersion,
    kind: admissionRequest.kind,  
    response: admissionResponse
  }

  res.send(200, admissionReview);
  log.Logger.info(`Validated request UID: ${admissionRequest.request.uid}`);
  next();
}

module.exports = { post }