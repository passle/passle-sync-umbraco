const defaultTheme = require("tailwindcss/defaultTheme");

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./PassleSync.Website/Views/**/*.cshtml"],
  theme: {
    container: {
      padding: {
        DEFAULT: "1rem",
        sm: "2rem",
        lg: "0rem",
      },
    },
    extend: {
      colors: {
        primary: "rgb(var(--color-primary) / <alpha-value>)",
        "primary-light": "rgb(var(--color-primary-light) / <alpha-value>)",
        linkedin: "#2867B2",
        twitter: "#1DA1F2",
        facebook: "#4267B2",
        xing: "#026466",
        secondary: "#026466",
        dark: "#1F2937",
        light: "#F9FAFB",
      },
      fontSize: {
        small: "0.875rem",
        regular: "1.5rem",
        large: "1.125rem",
        xl: "1.25rem",
        "2xl": "1.5rem",
        "3xl": "1.875rem",
      },
      fontFamily: {
        bodoni: ["Bodoni Moda", ...defaultTheme.fontFamily.sans],
      },
    },
    screens: {
      xs: "480px",
      sm: "600px",
      md: "782px",
      lg: "960px",
      xl: "1280px",
      "2xl": "1440px",
    },
  },
  plugins: [require("@tailwindcss/forms"), require("@tailwindcss/line-clamp")],
};
