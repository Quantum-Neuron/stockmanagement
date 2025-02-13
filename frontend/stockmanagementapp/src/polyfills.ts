/***************************************************************************************************
 * BROWSER POLYFILLS
 */

/**
 * This file provides polyfills for ES5 environments that may require them.
 * You may need to import other polyfills depending on your target environment.
 */

import 'core-js/es7/reflect'; // Required for Angular v13+ (and often needed for other libraries)

/**
 * Zone JS is required by Angular itself.
 */
import 'zone.js';  // Required for Angular


/***************************************************************************************************
 * APPLICATION IMPORTS
 */

/**
 * You might also want to include other polyfills here, depending on what features
 * your application uses and what browsers you need to support.  For example:
 *
 * import 'core-js/es6/symbol';
 * import 'core-js/es6/object';
 * import 'core-js/es6/function';
 * import 'core-js/es6/parse-int';
 * import 'core-js/es6/parse-float';
 * import 'core-js/es6/number';
 * import 'core-js/es6/math';
 * import 'core-js/es6/string';
 * import 'core-js/es6/array';
 * import 'core-js/es6/map';
 * import 'core-js/es6/set';
 *
 * If you need to support older browsers (especially IE), you might need to add these.
 * However, including many polyfills can increase your bundle size.  It's best to
 * only include the ones you actually need.
 */


/**
 *  If you're using `Promise`s, you might need this (not usually needed for modern Angular):
 *  import 'core-js/es6/promise';
 */

/**
 * If you're using `fetch` or other newer web APIs:
 * import 'whatwg-fetch';  // Or import the specific fetch polyfill you prefer
 */