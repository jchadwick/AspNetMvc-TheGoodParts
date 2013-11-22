    var MvcApp;
(function (MvcApp) {
    var MyClass = (function () {
        function MyClass(secret, notSecret) {
            this.secret = secret;
            this.notSecret = notSecret;
        }
        MyClass.prototype.toString = function () {
            return ['secret: ', this.secret, '; notSecret: ', this.notSecret].join('');
        };
        return MyClass;
    })();
    MvcApp.MyClass = MyClass;
})(MvcApp || (MvcApp = {}));

var instance = new MvcApp.MyClass('password', 1234);
instance.notSecret = 7890;
console.log(instance.toString());
//# sourceMappingURL=app.js.map
