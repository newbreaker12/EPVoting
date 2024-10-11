using System;
using System.Collections.Generic;
using System.Text;

namespace voting_bl.Service
{
    class EmailTemplates
    {
        public static string createTemplate = @"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <title>Voting System Email</title>
                <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1'>
                <!--[if mso]>
                <xml>
                    <o:OfficeDocumentSettings>
                        <o:PixelsPerInch>96</o:PixelsPerInch>
                        <o:AllowPNG/>
                    </o:OfficeDocumentSettings>
                </xml>
                <![endif]-->
                <style>
                    * {
                        box-sizing: border-box;
                    }
                    body {
                        margin: 0;
                        padding: 0;
                        -webkit-text-size-adjust: none;
                        text-size-adjust: none;
                        background-color: #ffffff;
                        font-family: Arial, Helvetica Neue, Helvetica, sans-serif;
                    }
                    a[x-apple-data-detectors] {
                        color: inherit !important;
                        text-decoration: inherit !important;
                    }
                    p {
                        line-height: 1.5;
                        margin: 0;
                    }
                    .desktop_hide,
                    .desktop_hide table {
                        mso-hide: all;
                        display: none;
                        max-height: 0;
                        overflow: hidden;
                    }
                    @media (max-width: 720px) {
                        .row-content {
                            width: 100% !important;
                        }
                        .mobile_hide {
                            display: none;
                        }
                        .stack .column {
                            width: 100%;
                            display: block;
                        }
                        .desktop_hide,
                        .desktop_hide table {
                            display: table !important;
                            max-height: none !important;
                        }
                    }
                </style>
            </head>
            <body>
                <table class='nl-container' width='100%' border='0' cellpadding='0' cellspacing='0' role='presentation'
                    style='background-color: #ffffff;'>
                    <tbody>
                        <tr>
                            <td>
                                <!-- Header Section -->
                                <table class='row row-1' align='center' width='100%' border='0' cellpadding='0' cellspacing='0'
                                    role='presentation'>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table class='row-content stack' align='center' border='0' cellpadding='0'
                                                    cellspacing='0' role='presentation' style='width: 700px;'>
                                                    <tbody>
                                                        <tr>
                                                            <td class='column column-1' width='100%'
                                                                style='padding-top: 30px; padding-bottom: 20px; text-align: center;'>
                                                                <p style='font-size: 24px; font-weight: bold;'>
                                                                    Voting System
                                                                </p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>

                                <!-- Subject Section -->
                                <table class='row row-2' align='center' width='100%' border='0' cellpadding='0' cellspacing='0'
                                    role='presentation' style='background-color: #4a65ad;'>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <table class='row-content stack' align='center' border='0' cellpadding='0'
                                                    cellspacing='0' role='presentation' style='width: 700px;'>
                                                    <tbody>
                                                        <tr>
                                                            <td class='column column-1' width='100%'
                                                                style='padding: 20px; text-align: center;'>
                                                                <p style='font-size: 24px; color: #ffffff; font-weight: bold;'>
                                                                    ::SUBJECT::
                                                                </p>
                                                            </td>
                                                        </tr>
                                                        <!-- Body Section -->
                                                        <tr>
                                                            <td class='column column-1' width='100%'
                                                                style='padding: 20px; text-align: center; background-color: #ffffff;'>
                                                                <p style='font-size: 16px; color: #333333;'>
                                                                    ::BODY::
                                                                </p>
                                                            </td>
                                                        </tr>
                                                        <!-- Signature Section -->
                                                        <tr>
                                                            <td class='column column-1' width='100%'
                                                                style='padding: 20px; text-align: left; background-color: #ffffff;'>
                                                                <p style='font-size: 14px; color: #333333;'>
                                                                    Best regards,<br>
                                                                    The Voting System Team
                                                                </p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </body>
            </html>";
    }
}
