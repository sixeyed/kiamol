const base64 = require('js-base64').Base64;
const log = require("./log");

function post(req, res, next) {
  log.Logger.debug("** POST /mutate called");

  var admissionRequest = req.body; 
  var object = admissionRequest.request.object;
  log.Logger.info(`Mutating object; request UID: ${admissionRequest.request.uid}`);
  
  var admissionReview = {
    apiVersion: admissionRequest.apiVersion,
    kind: admissionRequest.kind,    
    response: {
      uid: admissionRequest.request.uid,
      allowed: true
    }
  }  

  if (object.spec.hasOwnProperty("securityContext") &&
      object.spec.securityContext.hasOwnProperty("runAsNonRoot")) {
        log.Logger.info("- runAsNonRoot specified - no patch");
  }
  else {    
    let jsonPatch = [{
      op: "add",
      path: "/spec/securityContext/runAsNonRoot",
      value: true
    }];
    admissionReview.response.patch = base64.encode(JSON.stringify(jsonPatch));
    admissionReview.response.patchType = "JSONPatch"
    log.Logger.info("- added runAsNonRoot patch");
  }

  res.send(200, admissionReview);
  log.Logger.info(`Mutated request UID: ${admissionRequest.request.uid}`);
  next();
}

module.exports = { post }