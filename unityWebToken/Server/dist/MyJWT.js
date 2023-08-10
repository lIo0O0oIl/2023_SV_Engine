"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.decodeJWT = exports.createJWT = exports.tokenChecker = void 0;
const jsonwebtoken_1 = __importDefault(require("jsonwebtoken"));
const Secret_1 = require("./Secret");
const tokenChecker = (req, res, next) => __awaiter(void 0, void 0, void 0, function* () {
    let token = extractToken(req);
    if (token != undefined) {
        let decodedToken = (0, exports.decodeJWT)(token);
        if (decodedToken != null) {
            //console.log(token);
            let { id, email, xp, name } = (0, exports.decodeJWT)(token);
            req.user = { id, email, exp: xp, name };
        }
        else {
            req.user = null;
        }
    }
    else {
        req.user = null;
    }
    next();
});
exports.tokenChecker = tokenChecker;
function extractToken(req) {
    const PREFIX = "Bearer";
    const auth = req.headers.authorization;
    //프리픽스를 포함한 토큰이면 프리픽스를 제거하고, 그렇지 않다면 그냥 토큰 주고
    const token = (auth === null || auth === void 0 ? void 0 : auth.includes(PREFIX)) ? auth.split(PREFIX)[1] : auth;
    return token; //만약에 토큰이 존재하지 않았다면 undefined가 리턴될거다.
}
const createJWT = (userVO) => {
    const { id, email, name, exp } = userVO;
    const token = jsonwebtoken_1.default.sign({ id, email, name, xp: exp }, Secret_1.JWT_SECRET, { expiresIn: '7 days' });
    //10h, 100s, 50m 등 가능하다. 또는 아예 ms 단위로 계산해서 넣어줘도 된다. 
    return token;
};
exports.createJWT = createJWT;
const decodeJWT = (token) => {
    try {
        const decodedToken = jsonwebtoken_1.default.verify(token, Secret_1.JWT_SECRET); //넘어온 토큰을 디코드함.
        return decodedToken;
    }
    catch (e) {
        //console.log(e);
        return null;
    }
};
exports.decodeJWT = decodeJWT;
