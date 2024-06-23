using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SucessPointCore.Domain.Constants
{
    public class VerificationMailConstant
    {
        public const string SignupEmailVerificationContent = @"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Verify Your Account</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            background-color: #ffffff;
            padding: 20px 30px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            text-align: center;
            max-width: 500px;
            width: 100%;
            /* Border with curved corners */
            border-width: 15px;
            /* Adjust the thickness of the border */
            border-image-source: url('data:image/svg+xml;utf8,<svg xmlns=""http://www.w3.org/2000/svg"" width=""10"" height=""10""><rect x=""0"" y=""0"" width=""10"" height=""10"" rx=""3"" ry=""3"" fill=""green""/></svg>');
            /* SVG code for a green rounded rectangle */
            border-image-slice: 15 fill;
            /* Slice the image so that the corners remain rounded */
            border-image-repeat: round;
            /* Repeat the border image to fill the border */
        }

        .container h1 {
            color: #333333;
        }

        .container p {
            color: #555555;
            font-size: 16px;
            margin: 20px 0;
        }

        .verify-button {
            display: inline-block;
            padding: 12px 25px;
            background-color: #007BFF;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
            transition: background-color 0.3s;
        }

        .verify-button:hover {
            background-color: #0056b3;
        }

        .manual-link {
            margin-top: 20px;
            font-size: 14px;
            word-wrap: break-word;
            color: #007BFF;
            font.color: orange;
        }

        .footer {
            margin-top: 40px;
            color: #888888;
            font-size: 14px;
        }

        .footer p {
            margin: 5px 0;
        }

        .footer .footer-note {
            font-weight: bold;
            color: #333333;
            font-size: 16px;
        }
    </style>
</head>

<body>
    <div class=""container"">
        <h1>Verify Your Account</h1>
        <p>Thank you for registering with us! Please click the button below to verify your account.</p>
        <a href=""https://sp.premiersolution.in/api/verifyacc?VID={{VID}}&TFC={{TFC}}&TFC2={{TFC2}}"" class=""verify-button"">Verify Account</a>
        <p>If the button above is not clickable, please manually copy and paste the following link into your browser's address bar:</p>
        <p class=""manual-link"">https://sp.premiersolution.in/api/verifyacc?VID={{VID}}&TFC={{TFC}}&TFC2=45895623</p>
        <div class=""footer"">
            <p>If you did not request this registration, please ignore this email.</p>
            <p class=""footer-note"">Success Point: Making you succeed in your dreams.</p>
        </div>
    </div>
</body>

</html>";


        public const string ForgetPasswordVerificationContent = @"<!DOCTYPE html>
<html lang=""en"">

<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Verify Your Account</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }

        .container {
            background-color: #ffffff;
            padding: 20px 30px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            text-align: center;
            max-width: 500px;
            width: 100%;
            /* Border with curved corners */
            border-width: 15px;
            /* Adjust the thickness of the border */
            border-image-source: url('data:image/svg+xml;utf8,<svg xmlns=""http://www.w3.org/2000/svg"" width=""10"" height=""10""><rect x=""0"" y=""0"" width=""10"" height=""10"" rx=""3"" ry=""3"" fill=""green""/></svg>');
            /* SVG code for a green rounded rectangle */
            border-image-slice: 15 fill;
            /* Slice the image so that the corners remain rounded */
            border-image-repeat: round;
            /* Repeat the border image to fill the border */
        }

        .container h1 {
            color: #333333;
        }

        .container p {
            color: #555555;
            font-size: 16px;
            margin: 20px 0;
        }

        .verify-button {
            display: inline-block;
            padding: 12px 25px;
            background-color: #007BFF;
            color: #ffffff;
            text-decoration: none;
            border-radius: 5px;
            font-size: 16px;
            transition: background-color 0.3s;
        }

        .verify-button:hover {
            background-color: #0056b3;
        }

        .manual-link {
            margin-top: 20px;
            font-size: 14px;
            word-wrap: break-word;
            color: #007BFF;
            font.color: orange;
        }

        .footer {
            margin-top: 40px;
            color: #888888;
            font-size: 14px;
        }

        .footer p {
            margin: 5px 0;
        }

        .footer .footer-note {
            font-weight: bold;
            color: #333333;
            font-size: 16px;
        }
    </style>
</head>

<body>
    <div class=""container"">
        <h1>Verify Your Account</h1>
        <p>Thank you for registering with us! Please click the button below to Recover Your Password.</p>
        <a href=""https://sp.premiersolution.in/api/forgetacc?VID={{VID}}&TFC={{TFC}}&TFC2={{TFC2}}"" class=""verify-button"">Verify Account</a>
        <p>If the button above is not clickable, please manually copy and paste the following link into your browser's address bar:</p>
        <p class=""manual-link"">https://sp.premiersolution.in/api/forgetacc?VID={{VID}}&TFC={{TFC}}&TFC2=45895623</p>
        <div class=""footer"">
            <p>If you did not requested this forget password request, please ignore this email.</p>
            <p class=""footer-note"">Success Point: Making you succeed in your dreams.</p>
        </div>
    </div>
</body>

</html>";
    }
}
