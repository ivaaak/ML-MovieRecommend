const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/ML",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'https://localhost:7033',
        secure: false
    });

    app.use(appProxy);
};
