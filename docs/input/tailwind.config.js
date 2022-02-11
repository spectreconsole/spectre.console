const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = {
    content: ["./output/**/*.html"],
    darkMode: "class",
    theme: {
        extend: {
            fontFamily: {
                sans: ["Poppins", ...defaultTheme.fontFamily.sans],
                mono: ["ui-monospace", "Cascadia Mono", "Cascadia Code", "Menlo", "Consolas", "Liberation Mono", "Lucida Console", "WebCascadiaMonoPL", "monospace"],
            },
            container: ({theme}) => ({
                center: true,
                padding: {
                    DEFAULT: "2rem",
                    sm: "2rem",
                    lg: "4rem",
                    xl: "5rem",
                    "2xl": "6rem",
                },
                screens: {
                    sm: theme("spacing.full"),
                    md: theme("spacing.full"),
                    lg: "1280px",
                    xl: "1400px",
                },
            }),
            typography: (theme) => ({
                DEFAULT: {
                    css: {
                        h2: {
                            marginTop: '1.4em',
                            marginBottom: `.2em`,
                        },
                        h3: {
                            marginTop: '2.4em',
                            lineHeight: '1.4',
                        },
                        pre: {
                            fontWeight: theme("fontWeight.light"),
                            borderRadius: theme('borderRadius.xl'),
                            borderWidth: '1px',
                            borderColor: theme('colors.slate.700'),
                            color: theme('colors.slate.50'),
                            boxShadow: theme('boxShadow.md'),
                            lineHeight: '1.3',
                        },
                        'p + pre, p + asciinema-player pre': {
                            marginTop: `${-4 / 14}em`,
                        },
                        'pre + pre': {
                            marginTop: `${-16 / 14}em`,
                        },
                        code: {
                            fontWeight: theme("fontWeight.normal"),
                            fontSize: 'inherit',
                        },

                        "code::before": {
                            content: "&nbsp;",
                        },
                        "code::after": {
                            content: "&nbsp;",
                        },
                        td: {
                            overflowWrap: "anywhere",
                        },
                        a: {
                            fontWeight: theme('fontWeight.light'),
                            textDecoration: 'none',
                            borderBottom: `1px solid ${theme('colors.teal.600')}`,
                        },
                        'a:hover': {
                            borderBottomWidth: '2px',
                        },
                    },
                },
            }),
        },
    },
    variants: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
        require("@tailwindcss/typography")
    ],
};
