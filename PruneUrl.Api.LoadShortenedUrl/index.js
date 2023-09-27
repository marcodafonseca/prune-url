const aws = require('aws-sdk');
const dynamoDb = new aws.DynamoDB.DocumentClient({ region: 'eu-west-1' });
const { STATUS_CODES } = require('http');

const httpPrefixRegex = /(http|https):\/\/(.*)+/i;

exports.handler = async function (event, context) {
    var url = event.Records[0].cf.request.uri.substr(1);
    var response = {
        status: '404',
        headers: {
            'content-type': [{
                key: 'Content-Type',
                value: 'text/html'
            }],
            'content-encoding': [{
                key: 'Content-Encoding',
                value: 'UTF-8'
            }]
        },
        body: '<h1>404 - Page Not Found</h1>'
    };

    if (url) {
        console.log(`url: ${url}`);

        let dbQueryParams = {
            TableName: 'PrunedUrls',
            KeyConditionExpression: "#shortUrl = :shortUrl",
            ExpressionAttributeNames: {
                "#shortUrl": "ShortUrl"
            },
            ExpressionAttributeValues: {
                ':shortUrl': url
            }
        };

        var data = await dynamoDb.query(dbQueryParams).promise();

        if (data.Items.length > 0) {
            var responseUrl = data.Items[0].LongUrl;

            if (!httpPrefixRegex.test(responseUrl))
                responseUrl = `http://${responseUrl}`;

            response = {
                status: 301,
                headers: {
                    location: [{ key: 'Location', value: responseUrl }]
                }
            };
        }
    }

    response.statusDescription = STATUS_CODES[response.status];

    context.done(null, response);
};
