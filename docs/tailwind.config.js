const defaultTheme = require("tailwindcss/defaultTheme");
const colors = require("tailwindcss/colors");

module.exports = {
    purge: ["./output/**/*.html"],
    darkMode: false, // or 'media' or 'class'
    theme: {
        extend: {
            fontFamily: {
                sans: ["Poppins", ...defaultTheme.fontFamily.sans],
                mono: ["Cascadia Code", ...defaultTheme.fontFamily.mono],
            },
            container: {
                center: true,
                padding: {
                    DEFAULT: "2rem",
                    sm: "2rem",
                    lg: "4rem",
                    xl: "5rem",
                    "2xl": "6rem",
                },
            },
            typography: (theme) => ({
                DEFAULT: {
                    css: {
                        color: defaultTheme.colors.gray[900],
                        a: {
                            color: defaultTheme.colors.blue[700],
                            fontWeight: defaultTheme.fontWeight.normal,
                            "&:hover": {
                                color: defaultTheme.colors.blue[600],
                            },
                        },
                        "pre code": {
                            fontWeight: defaultTheme.fontWeight.light,
                        },
                        code: {
                            color: defaultTheme.colors.blue[900],
                            fontWeight: defaultTheme.fontWeight.normal,
                        },
                        "code::before": {
                            content: "&nbsp;",
                        },
                        "code::after": {
                            content: "&nbsp;",
                        },
                        h2: {
                            marginTop: "1em",
                            marginBottom: ".5em"
                        }
                    },
                },
            }),
        },
    },
    variants: {
        extend: {},
    },
    plugins: [require("@tailwindcss/typography")],
};
